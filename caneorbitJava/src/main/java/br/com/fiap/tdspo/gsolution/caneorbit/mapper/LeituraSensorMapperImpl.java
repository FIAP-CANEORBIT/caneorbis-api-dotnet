package br.com.fiap.tdspo.gsolution.caneorbit.mapper;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.LeituraSensorRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.LeituraSensorResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.LeituraSensor;
import org.springframework.stereotype.Component;

@Component
public class LeituraSensorMapperImpl implements LeituraSensorMapper {

    @Override
    public LeituraSensor toEntity(LeituraSensorRequestDTO dto) {
        if (dto == null) return null;

        LeituraSensor leitura = new LeituraSensor();
        leitura.setUmidadeSolo(dto.umidadeSolo());
        leitura.setTemperatura(dto.temperatura());
        leitura.setPhSolo(dto.phSolo());

        return leitura;
    }

    @Override
    public LeituraSensorResponseDTO toResponseDTO(LeituraSensor leitura) {
        if (leitura == null) return null;

        return new LeituraSensorResponseDTO(
                leitura.getId(),
                leitura.getUmidadeSolo(),
                leitura.getTemperatura(),
                leitura.getPhSolo(),
                leitura.getDataLeitura(),
                leitura.getDispositivo() != null ? leitura.getDispositivo().getId() : null,
                leitura.getDispositivo() != null ? leitura.getDispositivo().getMacAddress() : null,
                leitura.getDispositivo() != null && leitura.getDispositivo().getField() != null ? leitura.getDispositivo().getField().getNome() : null,
                leitura.getDispositivo() != null && leitura.getDispositivo().getField() != null && leitura.getDispositivo().getField().getPropriedade() != null ? leitura.getDispositivo().getField().getPropriedade().getNome() : null
        );
    }
}