package br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request;

import jakarta.validation.constraints.NotBlank;
import java.math.BigDecimal;
import java.time.LocalDate;

public record DispositivoIotRequestDTO(
        @NotBlank(message = "O MAC Address é obrigatório")
        String macAddress,

        String apelido,

        BigDecimal latitude,

        BigDecimal longitude,

        String statusDispositivo,

        LocalDate dataInstalacao
) {}