package br.com.fiap.tdspo.gsolution.caneorbit.domain.service;

import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request.PropriedadeRequestDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.api.dto.response.PropriedadeResponseDTO;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Propriedade;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Usuario;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.repository.PropriedadeRepository;
import br.com.fiap.tdspo.gsolution.caneorbit.domain.repository.UsuarioRepository;
import br.com.fiap.tdspo.gsolution.caneorbit.mapper.PropriedadeMapperImpl;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cache.annotation.CacheEvict;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

@Service
public class PropriedadeServiceImpl implements PropriedadeService {

    @Autowired
    private PropriedadeRepository propriedadeRepository;

    @Autowired
    private UsuarioRepository usuarioRepository;

    @Autowired
    private PropriedadeMapperImpl mapper;

    @Override
    @Cacheable("propriedades")
    public Page<PropriedadeResponseDTO> findAll(Pageable pageable) {
        return propriedadeRepository.findAll(pageable)
                .map(mapper::toResponseDTO);
    }

    @Override
    @Cacheable(value = "propriedades", key = "#id")
    public PropriedadeResponseDTO findById(Long id) {
        Propriedade propriedade = propriedadeRepository.findById(id).orElse(null);
        if (propriedade == null) {
            return null;
        }
        return mapper.toResponseDTO(propriedade);
    }

    @Override
    @Transactional
    @CacheEvict(value = "propriedades", allEntries = true)
    public PropriedadeResponseDTO create(PropriedadeRequestDTO dto, String usuarioEmail) {
        Usuario usuario = usuarioRepository.findByEmail(usuarioEmail).orElse(null);
        if (usuario == null) {
            return null;
        }

        Propriedade propriedade = mapper.toEntity(dto);
        propriedade.setUsuario(usuario);

        Propriedade salvo = propriedadeRepository.save(propriedade);

        return mapper.toResponseDTO(salvo);
    }

    @Override
    public Page<PropriedadeResponseDTO> findByUsuarioEmail(String email, Pageable pageable) {
        return propriedadeRepository.findByUsuarioEmail(email, pageable)
                .map(mapper::toResponseDTO);
    }

    @Override
    @Transactional
    @CacheEvict(value = "propriedades", allEntries = true)
    public PropriedadeResponseDTO update(Long id, PropriedadeRequestDTO dto, String usuarioEmail) {
        Propriedade propriedade = propriedadeRepository.findById(id).orElse(null);
        if (propriedade == null) {
            return null;
        }

        if (!propriedade.getUsuario().getEmail().equals(usuarioEmail)) {
            return null;
        }

        propriedade.setNome(dto.nome());
        propriedade.setLocalizacao(dto.localizacao());
        propriedade.setAreaHectare(dto.areaHectare());

        Propriedade atualizado = propriedadeRepository.save(propriedade);

        return mapper.toResponseDTO(atualizado);
    }

    @Override
    @Transactional
    @CacheEvict(value = "propriedades", allEntries = true)
    public void delete(Long id, String usuarioEmail) {
        Propriedade propriedade = propriedadeRepository.findById(id).orElse(null);
        if (propriedade == null) {
            return;
        }

        if (!propriedade.getUsuario().getEmail().equals(usuarioEmail)) {
            return;
        }

        propriedadeRepository.delete(propriedade);
    }
}