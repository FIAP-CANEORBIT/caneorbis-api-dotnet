package br.com.fiap.tdspo.gsolution.caneorbit.domain.repository;

import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Usuario;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.Optional;

@Repository
public interface UsuarioRepository extends JpaRepository<Usuario, Long> {
    Optional<Usuario> findByEmail(String email);
}