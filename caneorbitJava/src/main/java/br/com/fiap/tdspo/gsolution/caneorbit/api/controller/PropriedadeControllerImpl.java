package br.com.fiap.tdspo.gsolution.caneorbit.api.controller;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.PropriedadeRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.PropriedadeResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Field;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.repository.FieldRepository;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.service.PropriedadeService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/propriedades")
public class PropriedadeControllerImpl implements PropriedadeController {

    @Autowired
    private PropriedadeService propriedadeService;

    @Autowired
    private FieldRepository fieldRepository;

    @Override
    public ResponseEntity<Page<PropriedadeResponseDTO>> listarPropriedades(Pageable pageable) {
        Page<PropriedadeResponseDTO> propriedades = propriedadeService.findAll(pageable);
        return ResponseEntity.ok(propriedades);
    }

    @Override
    public ResponseEntity<PropriedadeResponseDTO> consultarPropriedadePorId(Long id) {
        PropriedadeResponseDTO propriedade = propriedadeService.findById(id);
        return ResponseEntity.ok(propriedade);
    }

    @Override
    public ResponseEntity<Page<PropriedadeResponseDTO>> listarMinhasPropriedades(UserDetails userDetails, Pageable pageable) {
        Page<PropriedadeResponseDTO> propriedades = propriedadeService.findByUsuarioEmail(userDetails.getUsername(), pageable);
        return ResponseEntity.ok(propriedades);
    }

    @Override
    public ResponseEntity<List<Field>> listarFieldsPorPropriedade(Long propriedadeId) {
        List<Field> fields = fieldRepository.findByPropriedadeId(propriedadeId);
        return ResponseEntity.ok(fields);
    }

    @Override
    public ResponseEntity<PropriedadeResponseDTO> criarPropriedade(PropriedadeRequestDTO dto, UserDetails userDetails) {
        PropriedadeResponseDTO novaPropriedade = propriedadeService.create(dto, userDetails.getUsername());
        return ResponseEntity.status(HttpStatus.CREATED).body(novaPropriedade);
    }

    @Override
    public ResponseEntity<PropriedadeResponseDTO> atualizarPropriedade(Long id, PropriedadeRequestDTO dto, UserDetails userDetails) {
        PropriedadeResponseDTO propriedadeAtualizada = propriedadeService.update(id, dto, userDetails.getUsername());
        return ResponseEntity.ok(propriedadeAtualizada);
    }

    @Override
    public ResponseEntity<Void> deletarPropriedade(Long id, UserDetails userDetails) {
        propriedadeService.delete(id, userDetails.getUsername());
        return ResponseEntity.noContent().build();
    }
}