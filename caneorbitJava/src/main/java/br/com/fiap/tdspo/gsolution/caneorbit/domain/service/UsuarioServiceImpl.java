package br.com.fiap.tdspo.gsolution.caneorbit.domain.service;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.UsuarioRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.UsuarioResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Usuario;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.repository.UsuarioRepository;
import br.com.fiap.tdspo.gsolution.caneorbit.mapper.UsuarioMapperImpl;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cache.annotation.CacheEvict;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

@Service
public class UsuarioServiceImpl implements UsuarioService {

    @Autowired
    private UsuarioRepository repository;

    @Autowired
    private UsuarioMapperImpl mapper;

    @Autowired
    private PasswordEncoder passwordEncoder;

    @Override
    @Cacheable("usuarios")
    public Page<UsuarioResponseDTO> findAll(Pageable pageable) {
        return repository.findAll(pageable)
                .map(mapper::toResponseDTO);
    }

    @Override
    @Cacheable(value = "usuarios", key = "#id")
    public UsuarioResponseDTO findById(Long id) {
        Usuario usuario = repository.findById(id)
                .orElseThrow(() -> new IllegalArgumentException("Usuário não encontrado com ID: " + id));
        return mapper.toResponseDTO(usuario);
    }

    @Override
    @Transactional
    @CacheEvict(value = "usuarios", allEntries = true)
    public UsuarioResponseDTO register(UsuarioRequestDTO dto) {
        Usuario usuario = mapper.toEntity(dto);

        String senhaCriptografada = passwordEncoder.encode(dto.senha());
        usuario.setSenhaHash(senhaCriptografada);

        Usuario salvo = repository.save(usuario);

        return mapper.toResponseDTO(salvo);
    }
}