package br.com.fiap.tdspo.gsolution.caneorbit.api.controller;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.DadosAutenticacaoDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.LoginResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Usuario;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.service.TokenService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/auth")
public class AutenticacaoControllerImpl {

    @Autowired
    private AuthenticationManager authenticationManager;

    @Autowired
    private TokenService tokenService;

    @PostMapping("/login")
    public ResponseEntity<LoginResponseDTO> login(@RequestBody @Valid DadosAutenticacaoDTO dados) {
        UsernamePasswordAuthenticationToken authenticationToken =
                new UsernamePasswordAuthenticationToken(dados.email(), dados.senha());

        Authentication authentication = authenticationManager.authenticate(authenticationToken);
        Usuario usuario = (Usuario) authentication.getPrincipal();

        String token = tokenService.generateToken(usuario);

        return ResponseEntity.ok(new LoginResponseDTO(
                token,
                "Bearer",
                usuario.getId(),
                usuario.getEmail(),
                usuario.getNome()
        ));
    }
}