package br.com.fiap.tdspo.gsolution.caneorbit.domain.service;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.PropriedadeRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.PropriedadeResponseDTO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

public interface PropriedadeService {
    Page<PropriedadeResponseDTO> findAll(Pageable pageable);
    PropriedadeResponseDTO findById(Long id);
    PropriedadeResponseDTO create(PropriedadeRequestDTO dto, String usuarioEmail);
    Page<PropriedadeResponseDTO> findByUsuarioEmail(String email, Pageable pageable);
    PropriedadeResponseDTO update(Long id, PropriedadeRequestDTO dto, String usuarioEmail);
    void delete(Long id, String usuarioEmail);
}