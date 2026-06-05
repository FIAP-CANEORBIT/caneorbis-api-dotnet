package br.com.fiap.tdspo.gsolution.caneorbit.domain.repository;

import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Propriedade;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface PropriedadeRepository extends JpaRepository<Propriedade, Long> {
    Page<Propriedade> findByUsuarioEmail(String email, Pageable pageable);
}