package br.com.fiap.tdspo.gsolution.caneorbit.mapper;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.PropriedadeRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.PropriedadeResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Propriedade;
import org.springframework.stereotype.Component;

@Component
public class PropriedadeMapperImpl implements PropriedadeMapper {

    @Override
    public Propriedade toEntity(PropriedadeRequestDTO dto) {
        if (dto == null) return null;

        Propriedade propriedade = new Propriedade();
        propriedade.setNome(dto.nome());
        propriedade.setLocalizacao(dto.localizacao());
        propriedade.setAreaHectare(dto.areaHectare());

        return propriedade;
    }

    @Override
    public PropriedadeResponseDTO toResponseDTO(Propriedade propriedade) {
        if (propriedade == null) return null;

        return new PropriedadeResponseDTO(
                propriedade.getId(),
                propriedade.getNome(),
                propriedade.getLocalizacao(),
                propriedade.getAreaHectare(),
                propriedade.getUsuario() != null ? propriedade.getUsuario().getId() : null,
                propriedade.getUsuario() != null ? propriedade.getUsuario().getNome() : null
        );
    }
}