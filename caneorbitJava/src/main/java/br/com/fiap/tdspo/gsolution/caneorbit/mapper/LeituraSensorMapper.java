package br.com.fiap.tdspo.gsolution.caneorbit.mapper;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.LeituraSensorRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.LeituraSensorResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.LeituraSensor;

public interface LeituraSensorMapper {
    LeituraSensor toEntity(LeituraSensorRequestDTO dto);
    LeituraSensorResponseDTO toResponseDTO(LeituraSensor leitura);
}