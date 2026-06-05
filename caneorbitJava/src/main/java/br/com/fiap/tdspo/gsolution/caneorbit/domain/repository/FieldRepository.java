package br.com.fiap.tdspo.gsolution.caneorbit.domain.repository;

import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Field;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface FieldRepository extends JpaRepository<Field, Long> {
    List<Field> findByPropriedadeId(Long propriedadeId);
}