package br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request;

import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Positive;
import java.math.BigDecimal;

public record LeituraSensorRequestDTO(
        @NotNull(message = "ID do dispositivo é obrigatório")
        Long idDispositivo,

        @NotNull(message = "A umidade do solo é obrigatória")
        @Positive(message = "A umidade deve ser um valor positivo")
        BigDecimal umidadeSolo,

        @NotNull(message = "A temperatura é obrigatória")
        BigDecimal temperatura,

        BigDecimal phSolo
) {}