package br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response;

import java.math.BigDecimal;
import java.time.LocalDateTime;

public record LeituraSensorResponseDTO(
        Long id,
        BigDecimal umidadeSolo,
        BigDecimal temperatura,
        BigDecimal phSolo,
        LocalDateTime dataLeitura,
        Long dispositivoId,
        String dispositivoMacAddress,
        String fieldNome,
        String propriedadeNome
) {}