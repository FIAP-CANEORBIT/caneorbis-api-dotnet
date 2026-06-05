package br.com.fiap.tdspo.gsolution.caneorbit.domain.service;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.DispositivoIotRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.DispositivoIotResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.DispositivoIot;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.repository.DispositivoIotRepository;
import br.com.fiap.tdspo.gsolution.caneorbit.mapper.DispositivoIotMapperImpl;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

@Service
public class DispositivoIotServiceImpl implements DispositivoIotService {

    @Autowired
    private DispositivoIotRepository dispositivoIotRepository;

    @Autowired
    private DispositivoIotMapperImpl mapper;

    @Override
    @Transactional
    public DispositivoIotResponseDTO create(DispositivoIotRequestDTO dto, String usuarioEmail) {
        if (dispositivoIotRepository.existsByMacAddress(dto.macAddress())) {
            throw new RuntimeException("MAC Address já cadastrado");
        }

        DispositivoIot dispositivo = mapper.toEntity(dto);
        dispositivo.setStatusDispositivo(dto.statusDispositivo() != null ? dto.statusDispositivo() : "ATIVO");

        DispositivoIot salvo = dispositivoIotRepository.save(dispositivo);
        return mapper.toResponseDTO(salvo);
    }

    @Override
    public Page<DispositivoIotResponseDTO> findByUsuarioEmail(String email, Pageable pageable) {
        return dispositivoIotRepository.findByFieldPropriedadeUsuarioEmail(email, pageable)
                .map(mapper::toResponseDTO);
    }

    @Override
    public DispositivoIotResponseDTO findById(Long id) {
        DispositivoIot dispositivo = dispositivoIotRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Dispositivo não encontrado"));
        return mapper.toResponseDTO(dispositivo);
    }

    @Override
    @Transactional
    public DispositivoIotResponseDTO update(Long id, DispositivoIotRequestDTO dto, String usuarioEmail) {
        DispositivoIot dispositivo = dispositivoIotRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Dispositivo não encontrado"));

        if (!dispositivo.getMacAddress().equals(dto.macAddress()) && dispositivoIotRepository.existsByMacAddress(dto.macAddress())) {
            throw new RuntimeException("MAC Address já existe");
        }

        dispositivo.setMacAddress(dto.macAddress());
        dispositivo.setApelido(dto.apelido());
        dispositivo.setLatitude(dto.latitude());
        dispositivo.setLongitude(dto.longitude());
        dispositivo.setStatusDispositivo(dto.statusDispositivo());
        dispositivo.setDataInstalacao(dto.dataInstalacao());

        DispositivoIot atualizado = dispositivoIotRepository.save(dispositivo);
        return mapper.toResponseDTO(atualizado);
    }

    @Override
    @Transactional
    public void delete(Long id, String usuarioEmail) {
        DispositivoIot dispositivo = dispositivoIotRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Dispositivo não encontrado"));
        dispositivoIotRepository.delete(dispositivo);
    }
}