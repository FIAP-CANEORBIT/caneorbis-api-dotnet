package br.com.fiap.tdspo.gsolution.caneorbit.domain.service;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.LeituraSensorRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.LeituraSensorResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.DispositivoIot;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.LeituraSensor;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.repository.DispositivoIotRepository;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.repository.LeituraSensorRepository;
import br.com.fiap.tdspo.gsolution.caneorbit.mapper.LeituraSensorMapperImpl;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

@Service
public class LeituraSensorServiceImpl implements LeituraSensorService {

    @Autowired
    private LeituraSensorRepository leituraSensorRepository;

    @Autowired
    private DispositivoIotRepository dispositivoIotRepository;

    @Autowired
    private LeituraSensorMapperImpl mapper;

    @Override
    @Transactional
    public LeituraSensorResponseDTO create(LeituraSensorRequestDTO dto) {
        DispositivoIot dispositivo = dispositivoIotRepository.findById(dto.idDispositivo()).orElse(null);
        if (dispositivo == null) {
            return null;
        }

        LeituraSensor leitura = mapper.toEntity(dto);
        leitura.setDispositivo(dispositivo);

        LeituraSensor salvo = leituraSensorRepository.save(leitura);
        return mapper.toResponseDTO(salvo);
    }

    @Override
    public Page<LeituraSensorResponseDTO> findByDispositivoId(Long dispositivoId, Pageable pageable) {
        return leituraSensorRepository.findByDispositivoId(dispositivoId, pageable)
                .map(mapper::toResponseDTO);
    }

    @Override
    public Page<LeituraSensorResponseDTO> findByUsuarioEmail(String email, Pageable pageable) {
        return leituraSensorRepository.findByDispositivoFieldPropriedadeUsuarioEmail(email, pageable)
                .map(mapper::toResponseDTO);
    }

    @Override
    public LeituraSensorResponseDTO findById(Long id) {
        LeituraSensor leitura = leituraSensorRepository.findById(id).orElse(null);
        if (leitura == null) {
            return null;
        }
        return mapper.toResponseDTO(leitura);
    }

    @Override
    @Transactional
    public void delete(Long id, String usuarioEmail) {
        LeituraSensor leitura = leituraSensorRepository.findById(id).orElse(null);
        if (leitura == null) {
            return;
        }

        if (!leitura.getDispositivo().getField().getPropriedade().getUsuario().getEmail().equals(usuarioEmail)) {
            return;
        }

        leituraSensorRepository.delete(leitura);
    }
}