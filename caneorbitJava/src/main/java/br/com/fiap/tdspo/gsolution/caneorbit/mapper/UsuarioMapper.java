package br.com.fiap.tdspo.gsolution.caneorbit.mapper;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.UsuarioRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.UsuarioResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Usuario;

public interface UsuarioMapper {
    Usuario toEntity(UsuarioRequestDTO dto);
    UsuarioResponseDTO toResponseDTO(Usuario usuario);
}
