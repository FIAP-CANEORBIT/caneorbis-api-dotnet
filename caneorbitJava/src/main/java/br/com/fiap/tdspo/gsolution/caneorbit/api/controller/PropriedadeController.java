package br.com.fiap.tdspo.gsolution.caneorbit.api.controller;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.PropriedadeRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.PropriedadeResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Field;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import org.springdoc.core.annotations.ParameterObject;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.web.PageableDefault;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@Tag(name = "Propriedades", description = "Endpoints para gerenciamento de propriedades agrícolas")
public interface PropriedadeController {

    @Operation(summary = "Listar propriedades", description = "Retorna uma lista paginada de todas as propriedades")
    @ApiResponses({
            @ApiResponse(responseCode = "200", description = "Lista retornada com sucesso"),
            @ApiResponse(responseCode = "400", description = "Parâmetros inválidos")
    })
    @GetMapping
    ResponseEntity<Page<PropriedadeResponseDTO>> listarPropriedades(
            @ParameterObject
            @PageableDefault(size = 10, sort = "id", direction = Sort.Direction.DESC) Pageable pageable
    );

    @Operation(summary = "Buscar propriedade por ID", description = "Retorna uma propriedade específica")
    @ApiResponses({
            @ApiResponse(responseCode = "200", description = "Propriedade encontrada"),
            @ApiResponse(responseCode = "404", description = "Propriedade não encontrada")
    })
    @GetMapping("/{id}")
    ResponseEntity<PropriedadeResponseDTO> consultarPropriedadePorId(@PathVariable Long id);

    @Operation(summary = "Listar minhas propriedades", description = "Retorna uma lista paginada das propriedades do usuário autenticado")
    @ApiResponses({
            @ApiResponse(responseCode = "200", description = "Lista retornada com sucesso"),
            @ApiResponse(responseCode = "401", description = "Usuário não autenticado")
    })
    @GetMapping("/minhas")
    ResponseEntity<Page<PropriedadeResponseDTO>> listarMinhasPropriedades(
            @AuthenticationPrincipal UserDetails userDetails,
            @ParameterObject
            @PageableDefault(size = 10, sort = "id", direction = Sort.Direction.DESC) Pageable pageable
    );

    @Operation(summary = "Listar fields de uma propriedade", description = "Retorna todos os fields de uma propriedade")
    @ApiResponses({
            @ApiResponse(responseCode = "200", description = "Lista retornada com sucesso"),
            @ApiResponse(responseCode = "404", description = "Propriedade não encontrada")
    })
    @GetMapping("/{propriedadeId}/fields")
    ResponseEntity<List<Field>> listarFieldsPorPropriedade(@PathVariable Long propriedadeId);

    @Operation(summary = "Criar propriedade", description = "Cadastra uma nova propriedade para o usuário autenticado")
    @ApiResponses({
            @ApiResponse(responseCode = "201", description = "Propriedade criada com sucesso"),
            @ApiResponse(responseCode = "400", description = "Dados inválidos"),
            @ApiResponse(responseCode = "401", description = "Usuário não autenticado")
    })
    @PostMapping
    ResponseEntity<PropriedadeResponseDTO> criarPropriedade(
            @RequestBody @Valid PropriedadeRequestDTO dto,
            @AuthenticationPrincipal UserDetails userDetails
    );

    @Operation(summary = "Atualizar propriedade", description = "Atualiza os dados de uma propriedade existente")
    @ApiResponses({
            @ApiResponse(responseCode = "200", description = "Propriedade atualizada com sucesso"),
            @ApiResponse(responseCode = "400", description = "Dados inválidos"),
            @ApiResponse(responseCode = "403", description = "Acesso negado"),
            @ApiResponse(responseCode = "404", description = "Propriedade não encontrada")
    })
    @PutMapping("/{id}")
    ResponseEntity<PropriedadeResponseDTO> atualizarPropriedade(
            @PathVariable Long id,
            @RequestBody @Valid PropriedadeRequestDTO dto,
            @AuthenticationPrincipal UserDetails userDetails
    );

    @Operation(summary = "Deletar propriedade", description = "Remove uma propriedade do sistema")
    @ApiResponses({
            @ApiResponse(responseCode = "204", description = "Propriedade deletada com sucesso"),
            @ApiResponse(responseCode = "403", description = "Acesso negado"),
            @ApiResponse(responseCode = "404", description = "Propriedade não encontrada")
    })
    @DeleteMapping("/{id}")
    ResponseEntity<Void> deletarPropriedade(
            @PathVariable Long id,
            @AuthenticationPrincipal UserDetails userDetails
    );
}