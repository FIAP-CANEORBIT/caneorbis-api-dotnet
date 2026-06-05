package br.com.fiap.tdspo.gsolution.caneorbit.mapper;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.PropriedadeRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.PropriedadeResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Propriedade;

public interface PropriedadeMapper {
    Propriedade toEntity(PropriedadeRequestDTO dto);
    PropriedadeResponseDTO toResponseDTO(Propriedade propriedade);
}