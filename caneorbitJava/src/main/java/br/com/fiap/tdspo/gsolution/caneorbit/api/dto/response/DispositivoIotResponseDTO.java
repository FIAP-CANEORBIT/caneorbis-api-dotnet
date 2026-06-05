package br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response;

import java.math.BigDecimal;
import java.time.LocalDate;

public record DispositivoIotResponseDTO(
        Long id,
        String macAddress,
        String apelido,
        BigDecimal latitude,
        BigDecimal longitude,
        String statusDispositivo,
        LocalDate dataInstalacao,
        Long fieldId,
        String fieldNome,
        Long propriedadeId,
        String propriedadeNome
) {}