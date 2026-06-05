package br.com.fiap.tdspo.gsolution.caneorbit.mapper;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.UsuarioRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.UsuarioResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Usuario;
import org.springframework.stereotype.Component;

@Component
public class UsuarioMapperImpl implements UsuarioMapper {

    @Override
    public Usuario toEntity(UsuarioRequestDTO dto) {
        if (dto == null) return null;

        Usuario usuario = new Usuario();
        usuario.setNome(dto.nome());
        usuario.setEmail(dto.email());
        usuario.setSenhaHash(dto.senha());

        return usuario;
    }

    @Override
    public UsuarioResponseDTO toResponseDTO(Usuario usuario) {
        if (usuario == null) return null;

        return new UsuarioResponseDTO(
                usuario.getId(),
                usuario.getNome(),
                usuario.getEmail(),
                usuario.getDataCadastro()
        );
    }
}