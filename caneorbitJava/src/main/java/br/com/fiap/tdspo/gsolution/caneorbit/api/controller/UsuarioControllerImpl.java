package br.com.fiap.tdspo.gsolution.caneorbit.api.controller;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.UsuarioRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.UsuarioResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.service.UsuarioService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/usuarios")
public class UsuarioControllerImpl implements UsuarioController {

    @Autowired
    private UsuarioService service;

    @GetMapping
    @Override
    public ResponseEntity<Page<UsuarioResponseDTO>> listarUsuarios(Pageable pageable) {
        Page<UsuarioResponseDTO> usuarios = service.findAll(pageable);
        return ResponseEntity.ok(usuarios);
    }

    @GetMapping("/{id}")
    @Override
    public ResponseEntity<UsuarioResponseDTO> consultarUsuarioPorId(@PathVariable Long id) {
        UsuarioResponseDTO usuario = service.findById(id);
        return ResponseEntity.ok(usuario);
    }

    @PostMapping("/register")
    @Override
    public ResponseEntity<UsuarioResponseDTO> criarUsuario(@RequestBody @Valid UsuarioRequestDTO dto) {
        UsuarioResponseDTO novoUsuario = service.register(dto);
        return ResponseEntity.status(HttpStatus.CREATED).body(novoUsuario);
    }
}