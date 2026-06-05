package br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response;
import java.time.LocalDateTime;

public record ErroResponseDTO(
        LocalDateTime timestamp,
        int status,
        String error,
        String message,
        String path
) {}
