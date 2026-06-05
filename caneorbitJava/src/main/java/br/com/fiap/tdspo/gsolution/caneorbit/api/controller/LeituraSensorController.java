package br.com.fiap.tdspo.gsolution.caneorbit.api.controller;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.LeituraSensorRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.LeituraSensorResponseDTO;
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

@Tag(name = "Leituras de Sensores", description = "Endpoints para gerenciamento de leituras de sensores IoT")
public interface LeituraSensorController {

    @Operation(summary = "Criar leitura", description = "Registra uma nova leitura de sensor (enviada pelo ESP32)")
    @ApiResponses({
            @ApiResponse(responseCode = "201", description = "Leitura criada com sucesso"),
            @ApiResponse(responseCode = "400", description = "Dados inválidos"),
            @ApiResponse(responseCode = "404", description = "Dispositivo não encontrado")
    })
    @PostMapping
    ResponseEntity<LeituraSensorResponseDTO> criarLeitura(@RequestBody @Valid LeituraSensorRequestDTO dto);

    @Operation(summary = "Listar leituras por dispositivo", description = "Retorna uma lista paginada de leituras de um dispositivo")
    @GetMapping("/dispositivo/{dispositivoId}")
    ResponseEntity<Page<LeituraSensorResponseDTO>> listarLeiturasPorDispositivo(
            @PathVariable Long dispositivoId,
            @ParameterObject
            @PageableDefault(size = 20, sort = "dataLeitura", direction = Sort.Direction.DESC) Pageable pageable
    );

    @Operation(summary = "Buscar leitura por ID", description = "Retorna uma leitura específica")
    @GetMapping("/{id}")
    ResponseEntity<LeituraSensorResponseDTO> buscarLeituraPorId(@PathVariable Long id);

    @Operation(summary = "Listar minhas leituras", description = "Retorna uma lista paginada de leituras dos meus dispositivos")
    @GetMapping("/minhas")
    ResponseEntity<Page<LeituraSensorResponseDTO>> listarMinhasLeituras(
            @AuthenticationPrincipal UserDetails userDetails,
            @ParameterObject
            @PageableDefault(size = 20, sort = "dataLeitura", direction = Sort.Direction.DESC) Pageable pageable
    );

    @Operation(summary = "Deletar leitura", description = "Remove uma leitura do sistema")
    @DeleteMapping("/{id}")
    ResponseEntity<Void> deletarLeitura(
            @PathVariable Long id,
            @AuthenticationPrincipal UserDetails userDetails
    );
}