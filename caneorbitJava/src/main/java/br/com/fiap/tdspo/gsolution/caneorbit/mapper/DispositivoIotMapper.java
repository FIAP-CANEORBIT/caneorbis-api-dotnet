package br.com.fiap.tdspo.gsolution.caneorbit.mapper;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.DispositivoIotRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.DispositivoIotResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.DispositivoIot;

public interface DispositivoIotMapper {
    DispositivoIot toEntity(DispositivoIotRequestDTO dto);
    DispositivoIotResponseDTO toResponseDTO(DispositivoIot dispositivo);
}