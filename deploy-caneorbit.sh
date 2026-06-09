#!/bin/bash

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

# Configurações
RM="566385"

clear
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}🚀 CaneOrbit - Criar VM na Azure${NC}"
echo -e "${GREEN}========================================${NC}"

# ============================================================
# ETAPA 1: LOGIN AZURE
# ============================================================
echo -e "\n${BLUE}[1/5] Verificando login no Azure...${NC}"
if ! az account show &> /dev/null; then
    echo -e "${RED}❌ Faça login primeiro: az login${NC}"
    exit 1
fi
echo -e "${GREEN}✅ Logado no Azure${NC}"

# ============================================================
# ETAPA 2: CONFIGURAÇÕES
# ============================================================
echo -e "\n${BLUE}[2/5] Configurações da VM...${NC}"
read -p "Nome do Grupo de Recursos (ex: rg-caneorbit): " RG
RG=${RG:-rg-caneorbit}

echo "Escolha o tamanho da VM:"
select s in "Standard_B2s (2 vCPU, 4GB - Econômico)" "Standard_D2s_v3 (2 vCPU, 8GB - Recomendado)" "Standard_E2s_v3 (2 vCPU, 16GB - Memória)"; do 
    case $s in 
        "Standard_B2s (2 vCPU, 4GB - Econômico)") size="Standard_B2s"; break;; 
        "Standard_D2s_v3 (2 vCPU, 8GB - Recomendado)") size="Standard_D2s_v3"; break;; 
        "Standard_E2s_v3 (2 vCPU, 16GB - Memória)") size="Standard_E2s_v3"; break;; 
    esac
done

VM_NAME="vm-caneorbit-api"
USER="caneorbitadmin"
PASSWORD="CaneOrbit@2026!$(date +%s | tail -c 4)"
DNS_NAME="caneorbit-$(date +%s)"

REGIOES_ALVO=("canadacentral" "canadaeast" "mexicocentral" "eastus" "brazilsouth" "westus")
REGIAO_DISPONIVEL=""

echo -e "\n${YELLOW}🔍 Buscando região disponível para $size...${NC}"
for loc in "${REGIOES_ALVO[@]}"; do
    echo -n "   Testando $loc... "
    RESTRICTION=$(az vm list-skus -l $loc --size $size --all --query "[?name=='$size'].restrictions[].type" -o tsv)
    if [[ -z "$RESTRICTION" ]]; then
        echo -e "${GREEN}Disponível! ✅${NC}"
        REGIAO_DISPONIVEL=$loc
        break
    else
        echo -e "${RED}Bloqueado ❌${NC}"
    fi
done

if [[ -z "$REGIAO_DISPONIVEL" ]]; then
    echo -e "\n${RED}❌ Nenhuma região disponível para $size.${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Região selecionada: $REGIAO_DISPONIVEL${NC}"

# ============================================================
# ETAPA 3: CRIAR VM
# ============================================================
echo -e "\n${BLUE}[3/5] Criando Resource Group...${NC}"
az group create --name $RG --location $REGIAO_DISPONIVEL -o none
echo -e "${GREEN}✅ Resource Group criado: $RG${NC}"

echo -e "\n${BLUE}[4/5] Criando VNet...${NC}"
VNET_NAME="vnet-caneorbit"
SUBNET_NAME="subnet-caneorbit"
az network vnet create \
    --resource-group $RG \
    --name $VNET_NAME \
    --address-prefix 10.0.0.0/16 \
    --subnet-name $SUBNET_NAME \
    --subnet-prefix 10.0.1.0/24 \
    -o none
echo -e "${GREEN}✅ VNet criada${NC}"

echo -e "\n${BLUE}[5/5] Criando VM (pode levar alguns minutos)...${NC}"
az vm create \
    --resource-group $RG \
    --name $VM_NAME \
    --image Ubuntu2204 \
    --size $size \
    --admin-username $USER \
    --admin-password $PASSWORD \
    --public-ip-address-dns-name $DNS_NAME \
    --location $REGIAO_DISPONIVEL \
    --vnet-name $VNET_NAME \
    --subnet $SUBNET_NAME

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✅ VM criada com sucesso!${NC}"
else
    echo -e "${RED}❌ Falha ao criar VM. Limpando...${NC}"
    az group delete --name $RG --yes --no-wait
    exit 1
fi

# ============================================================
# ETAPA 4: LIBERAR PORTAS
# ============================================================
echo -e "\n${BLUE}[6/5] Liberando portas no firewall...${NC}"
az network nsg rule create -g $RG --nsg-name ${VM_NAME}NSG --name allow-ssh --protocol tcp --priority 1000 --destination-port-range 22 -o none
az network nsg rule create -g $RG --nsg-name ${VM_NAME}NSG --name allow-java --protocol tcp --priority 1010 --destination-port-range 8080 -o none
az network nsg rule create -g $RG --nsg-name ${VM_NAME}NSG --name allow-csharp --protocol tcp --priority 1020 --destination-port-range 5000 -o none
az network nsg rule create -g $RG --nsg-name ${VM_NAME}NSG --name allow-oracle --protocol tcp --priority 1030 --destination-port-range 1521 -o none
echo -e "${GREEN}✅ Portas 22, 8080, 5000, 1521 liberadas${NC}"

# ============================================================
# ETAPA 5: OBTER IP
# ============================================================
IP=$(az vm show -d -g $RG -n $VM_NAME --query publicIps -o tsv)

# ============================================================
# MOSTRA INFORMAÇÕES
# ============================================================
clear
echo -e "\n${GREEN}══════════════════════════════════════════════════════════════════════════${NC}"
echo -e "${GREEN}✅ VM CRIADA COM SUCESSO!${NC}"
echo -e "${GREEN}══════════════════════════════════════════════════════════════════════════${NC}"
echo -e ""
echo -e "${YELLOW}📋 INFORMAÇÕES DA VM:${NC}"
echo -e "   ┌─────────────────────────────────────────────────────────────┐"
echo -e "   │ ${BLUE}Resource Group:${NC}      $RG"
echo -e "   │ ${BLUE}Região:${NC}               $REGIAO_DISPONIVEL"
echo -e "   │ ${BLUE}Tamanho VM:${NC}           $size"
echo -e "   │ ${BLUE}Nome da VM:${NC}           $VM_NAME"
echo -e "   │ ${BLUE}Usuário:${NC}              $USER"
echo -e "   │ ${BLUE}Senha SSH:${NC}            ${GREEN}$PASSWORD${NC}"
echo -e "   │ ${BLUE}IP Público:${NC}           ${GREEN}$IP${NC}"
echo -e "   │ ${BLUE}DNS:${NC}                  $DNS_NAME"
echo -e "   └─────────────────────────────────────────────────────────────┘"
echo -e ""
echo -e "${YELLOW}🔗 COMANDOS PARA ACESSAR:${NC}"
echo -e "   ┌─────────────────────────────────────────────────────────────┐"
echo -e "   │ ${BLUE}SSH:${NC}                   ssh $USER@$IP"
echo -e "   │ ${BLUE}Senha:${NC}                $PASSWORD"
echo -e "   └─────────────────────────────────────────────────────────────┘"
echo -e ""
echo -e "${YELLOW}🌐 ENDPOINTS (após instalar Docker):${NC}"
echo -e "   ┌─────────────────────────────────────────────────────────────┐"
echo -e "   │ ${BLUE}API Java:${NC}              http://$IP:8080"
echo -e "   │ ${BLUE}Swagger Java:${NC}          http://$IP:8080/swagger-ui/index.html"
echo -e "   │ ${BLUE}API C#:${NC}                http://$IP:5000"
echo -e "   │ ${BLUE}Swagger C#:${NC}            http://$IP:5000/swagger"
echo -e "   └─────────────────────────────────────────────────────────────┘"
echo -e ""
echo -e "${GREEN}══════════════════════════════════════════════════════════════════════════${NC}"
echo -e "${YELLOW}💾 Salve essas informações! A senha não será mostrada novamente.${NC}"
echo -e "${GREEN}══════════════════════════════════════════════════════════════════════════${NC}"

# Salvar em arquivo
cat > ~/caneorbit-vm-info.txt << EOF
========================================
CANEORBIT - INFORMAÇÕES DA VM
========================================
Data: $(date)
Resource Group: $RG
Região: $REGIAO_DISPONIVEL
Tamanho: $size
VM Name: $VM_NAME
Usuário: $USER
Senha: $PASSWORD
IP: $IP
DNS: $DNS_NAME
========================================
Comando SSH: ssh $USER@$IP
========================================
API Java: http://$IP:8080
Swagger Java: http://$IP:8080/swagger-ui/index.html
API C#: http://$IP:5000
Swagger C#: http://$IP:5000/swagger
========================================
EOF

echo -e "${GREEN}✅ Informações salvas em: ~/caneorbit-vm-info.txt${NC}"
echo -e ""
echo -e "${YELLOW}📝 Próximos passos manuais:${NC}"
echo -e "   1. ssh $USER@$IP"
echo -e "   2. Instalar Docker (curl -fsSL https://get.docker.com | sudo sh)"
echo -e "   3. sudo usermod -aG docker \$USER && newgrp docker"
echo -e "   4. git clone https://github.com/FIAP-CANEORBIT/caneorbis-api-dotnet.git"
echo -e "   5. cd caneorbis-api-dotnet && docker compose up -d --build"
echo -e ""

# Menu de gerenciamento
echo -e "${YELLOW}Controle de Ciclo de Vida${NC}"
select a in "Sair (manter VM rodando)" "Parar VM" "Desalocar VM" "Iniciar VM" "Deletar Tudo"; do 
    case $a in 
        "Sair (manter VM rodando)") 
            echo -e "${GREEN}✅ VM mantida em execução.${NC}"
            break
            ;;
        "Parar VM") 
            az vm stop --resource-group $RG --name $VM_NAME
            echo -e "${GREEN}✅ VM parada${NC}"
            break
            ;; 
        "Desalocar VM") 
            az vm deallocate --resource-group $RG --name $VM_NAME
            echo -e "${GREEN}✅ VM desalocada (sem custos)${NC}"
            break
            ;; 
        "Iniciar VM") 
            az vm start --resource-group $RG --name $VM_NAME
            IP=$(az vm show -d -g $RG -n $VM_NAME --query publicIps -o tsv)
            echo -e "${GREEN}✅ VM iniciada - IP: $IP${NC}"
            break
            ;; 
        "Deletar Tudo") 
            az group delete --name $RG --yes --no-wait
            echo -e "${GREEN}✅ RG deletado${NC}"
            break
            ;;
    esac
done