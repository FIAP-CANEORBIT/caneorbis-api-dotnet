package br.com.fiap.tdspo.gsolution.caneorbit.domain.service;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.UsuarioRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.UsuarioResponseDTO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

public interface UsuarioService {
    Page<UsuarioResponseDTO> findAll(Pageable pageable);
    UsuarioResponseDTO findById(Long id);
    UsuarioResponseDTO register(UsuarioRequestDTO dto);
}
