package br.com.fiap.tdspo.gsolution.caneorbit.api.controller;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.LeituraSensorRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.LeituraSensorResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.service.LeituraSensorService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/leituras")
public class LeituraSensorControllerImpl implements LeituraSensorController {

    @Autowired
    private LeituraSensorService leituraSensorService;

    @Override
    public ResponseEntity<LeituraSensorResponseDTO> criarLeitura(LeituraSensorRequestDTO dto) {
        LeituraSensorResponseDTO novaLeitura = leituraSensorService.create(dto);
        return ResponseEntity.status(HttpStatus.CREATED).body(novaLeitura);
    }

    @Override
    public ResponseEntity<Page<LeituraSensorResponseDTO>> listarLeiturasPorDispositivo(Long dispositivoId, Pageable pageable) {
        Page<LeituraSensorResponseDTO> leituras = leituraSensorService.findByDispositivoId(dispositivoId, pageable);
        return ResponseEntity.ok(leituras);
    }

    @Override
    public ResponseEntity<LeituraSensorResponseDTO> buscarLeituraPorId(Long id) {
        LeituraSensorResponseDTO leitura = leituraSensorService.findById(id);
        return ResponseEntity.ok(leitura);
    }

    @Override
    public ResponseEntity<Page<LeituraSensorResponseDTO>> listarMinhasLeituras(UserDetails userDetails, Pageable pageable) {
        Page<LeituraSensorResponseDTO> leituras = leituraSensorService.findByUsuarioEmail(userDetails.getUsername(), pageable);
        return ResponseEntity.ok(leituras);
    }

    @Override
    public ResponseEntity<Void> deletarLeitura(Long id, UserDetails userDetails) {
        leituraSensorService.delete(id, userDetails.getUsername());
        return ResponseEntity.noContent().build();
    }
}