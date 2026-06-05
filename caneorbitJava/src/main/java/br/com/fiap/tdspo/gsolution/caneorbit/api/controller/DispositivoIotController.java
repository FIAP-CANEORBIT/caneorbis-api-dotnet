package br.com.fiap.tdspo.gsolution.caneorbit.api.controller;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.DispositivoIotRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.DispositivoIotResponseDTO;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;

@Tag(name = "Dispositivos IoT", description = "Endpoints para gerenciamento de dispositivos IoT")
public interface DispositivoIotController {

    @PostMapping
    ResponseEntity<DispositivoIotResponseDTO> criarDispositivo(
            @RequestBody @Valid DispositivoIotRequestDTO dto,
            @AuthenticationPrincipal UserDetails userDetails);

    @GetMapping("/{id}")
    ResponseEntity<DispositivoIotResponseDTO> buscarDispositivoPorId(@PathVariable Long id);

    @GetMapping("/meus")
    ResponseEntity<Page<DispositivoIotResponseDTO>> listarMeusDispositivos(
            @AuthenticationPrincipal UserDetails userDetails,
            Pageable pageable);

    @PutMapping("/{id}")
    ResponseEntity<DispositivoIotResponseDTO> atualizarDispositivo(
            @PathVariable Long id,
            @RequestBody @Valid DispositivoIotRequestDTO dto,
            @AuthenticationPrincipal UserDetails userDetails);

    @DeleteMapping("/{id}")
    ResponseEntity<Void> deletarDispositivo(
            @PathVariable Long id,
            @AuthenticationPrincipal UserDetails userDetails);
}