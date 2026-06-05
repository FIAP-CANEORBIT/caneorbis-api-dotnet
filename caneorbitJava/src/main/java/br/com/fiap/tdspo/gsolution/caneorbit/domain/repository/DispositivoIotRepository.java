package br.com.fiap.tdspo.gsolution.caneorbit.domain.repository;

import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.DispositivoIot;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface DispositivoIotRepository extends JpaRepository<DispositivoIot, Long> {
    Page<DispositivoIot> findByFieldPropriedadeUsuarioEmail(String email, Pageable pageable);
    boolean existsByMacAddress(String macAddress);
}