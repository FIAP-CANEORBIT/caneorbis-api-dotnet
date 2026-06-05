package br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response;

public record LoginResponseDTO(
        String token,
        String tipo,
        Long id,
        String email,
        String nome
) {}
