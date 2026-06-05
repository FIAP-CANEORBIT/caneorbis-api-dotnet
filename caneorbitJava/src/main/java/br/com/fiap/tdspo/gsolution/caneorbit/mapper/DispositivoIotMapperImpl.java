package br.com.fiap.tdspo.gsolution.caneorbit.mapper;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.DispositivoIotRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.DispositivoIotResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.DispositivoIot;
import org.springframework.stereotype.Component;

@Component
public class DispositivoIotMapperImpl implements DispositivoIotMapper {

    @Override
    public DispositivoIot toEntity(DispositivoIotRequestDTO dto) {
        if (dto == null) return null;

        DispositivoIot dispositivo = new DispositivoIot();
        dispositivo.setMacAddress(dto.macAddress());
        dispositivo.setApelido(dto.apelido());
        dispositivo.setLatitude(dto.latitude());
        dispositivo.setLongitude(dto.longitude());
        dispositivo.setStatusDispositivo(dto.statusDispositivo());
        dispositivo.setDataInstalacao(dto.dataInstalacao());

        return dispositivo;
    }

    @Override
    public DispositivoIotResponseDTO toResponseDTO(DispositivoIot dispositivo) {
        if (dispositivo == null) return null;

        return new DispositivoIotResponseDTO(
                dispositivo.getId(),
                dispositivo.getMacAddress(),
                dispositivo.getApelido(),
                dispositivo.getLatitude(),
                dispositivo.getLongitude(),
                dispositivo.getStatusDispositivo(),
                dispositivo.getDataInstalacao(),
                dispositivo.getField() != null ? dispositivo.getField().getId() : null,
                dispositivo.getField() != null ? dispositivo.getField().getNome() : null,
                dispositivo.getField() != null && dispositivo.getField().getPropriedade() != null ? dispositivo.getField().getPropriedade().getId() : null,
                dispositivo.getField() != null && dispositivo.getField().getPropriedade() != null ? dispositivo.getField().getPropriedade().getNome() : null
        );
    }
}