package br.com.fiap.tdspo.gsolution.caneorbit.domain.service;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.LeituraSensorRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.LeituraSensorResponseDTO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

public interface LeituraSensorService {
    LeituraSensorResponseDTO create(LeituraSensorRequestDTO dto);
    Page<LeituraSensorResponseDTO> findByDispositivoId(Long dispositivoId, Pageable pageable);
    Page<LeituraSensorResponseDTO> findByUsuarioEmail(String email, Pageable pageable);
    LeituraSensorResponseDTO findById(Long id);
    void delete(Long id, String usuarioEmail);
}