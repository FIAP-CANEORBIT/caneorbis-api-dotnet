package br.com.fiap.tdspo.gsolution.caneorbit.domain.service;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.DispositivoIotRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.DispositivoIotResponseDTO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

public interface DispositivoIotService {
    DispositivoIotResponseDTO create(DispositivoIotRequestDTO dto, String usuarioEmail);
    Page<DispositivoIotResponseDTO> findByUsuarioEmail(String email, Pageable pageable);
    DispositivoIotResponseDTO findById(Long id);
    DispositivoIotResponseDTO update(Long id, DispositivoIotRequestDTO dto, String usuarioEmail);
    void delete(Long id, String usuarioEmail);
}