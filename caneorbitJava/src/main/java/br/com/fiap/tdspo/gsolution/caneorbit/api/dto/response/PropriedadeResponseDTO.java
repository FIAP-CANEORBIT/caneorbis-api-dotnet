package br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response;

import java.math.BigDecimal;

public record PropriedadeResponseDTO(
        Long id,
        String nome,
        String localizacao,
        BigDecimal areaHectare,
        Long usuarioId,
        String usuarioNome
) {}
