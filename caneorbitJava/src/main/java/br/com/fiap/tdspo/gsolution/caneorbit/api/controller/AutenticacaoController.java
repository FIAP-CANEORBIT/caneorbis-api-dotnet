package br.com.fiap.tdspo.gsolution.caneorbit.api.controller;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.DadosAutenticacaoDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.LoginResponseDTO;
import jakarta.validation.Valid;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

public interface AutenticacaoController {
    @PostMapping
    ResponseEntity<LoginResponseDTO> efetuarLogin(@RequestBody @Valid DadosAutenticacaoDTO dados);
}
