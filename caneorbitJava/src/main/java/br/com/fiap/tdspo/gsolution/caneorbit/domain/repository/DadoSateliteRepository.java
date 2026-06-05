package br.com.fiap.tdspo.gsolution.caneorbit.domain.repository;

import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.DadoSatelite;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface DadoSateliteRepository extends JpaRepository<DadoSatelite, Long> {
    List<DadoSatelite> findByDispositivoId(Long dispositivoId);
}