package br.com.fiap.tdspo.gsolution.caneorbit.domain.repository;

import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.LeituraSensor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface LeituraSensorRepository extends JpaRepository<LeituraSensor, Long> {
    Page<LeituraSensor> findByDispositivoId(Long dispositivoId, Pageable pageable);
    Page<LeituraSensor> findByDispositivoFieldPropriedadeUsuarioEmail(String email, Pageable pageable);
}