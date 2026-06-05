package br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response;

import java.time.LocalDateTime;

public record UsuarioResponseDTO(
        Long id,
        String nome,
        String email,
        LocalDateTime dataCadastro
) {}
