package br.com.fiap.tdspo.gsolution.caneorbit.api.controller;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.DispositivoIotRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.DispositivoIotResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.service.DispositivoIotService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/dispositivos")
public class DispositivoIotControllerImpl implements DispositivoIotController {

    @Autowired
    private DispositivoIotService dispositivoIotService;

    @PostMapping
    public ResponseEntity<DispositivoIotResponseDTO> criarDispositivo(
            @RequestBody @Valid DispositivoIotRequestDTO dto,
            @AuthenticationPrincipal UserDetails userDetails) {
        DispositivoIotResponseDTO novoDispositivo = dispositivoIotService.create(dto, userDetails.getUsername());
        return ResponseEntity.status(HttpStatus.CREATED).body(novoDispositivo);
    }

    @GetMapping("/{id}")
    public ResponseEntity<DispositivoIotResponseDTO> buscarDispositivoPorId(@PathVariable Long id) {
        return ResponseEntity.ok(dispositivoIotService.findById(id));
    }

    @GetMapping("/meus")
    public ResponseEntity<Page<DispositivoIotResponseDTO>> listarMeusDispositivos(
            @AuthenticationPrincipal UserDetails userDetails,
            Pageable pageable) {
        return ResponseEntity.ok(dispositivoIotService.findByUsuarioEmail(userDetails.getUsername(), pageable));
    }

    @PutMapping("/{id}")
    public ResponseEntity<DispositivoIotResponseDTO> atualizarDispositivo(
            @PathVariable Long id,
            @RequestBody @Valid DispositivoIotRequestDTO dto,
            @AuthenticationPrincipal UserDetails userDetails) {
        return ResponseEntity.ok(dispositivoIotService.update(id, dto, userDetails.getUsername()));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deletarDispositivo(
            @PathVariable Long id,
            @AuthenticationPrincipal UserDetails userDetails) {
        dispositivoIotService.delete(id, userDetails.getUsername());
        return ResponseEntity.noContent().build();
    }
}