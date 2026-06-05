package br.com.fiap.tdspo.gsolution.caneorbit.domain.model;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "T_ORB_DADO_SATELITE")
public class DadoSatelite {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ID_DADO_SATELITE", nullable = false)
    private Long id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "ID_DISPOSITIVO", nullable = false)
    private DispositivoIot dispositivo;

    @Column(name = "VL_NDVI", precision = 10, scale = 4)
    private BigDecimal ndvi;

    @Column(name = "VL_PRECIPITACAO", precision = 10, scale = 2)
    private BigDecimal precipitacao;

    @Column(name = "VL_TEMPERATURA_AR", precision = 10, scale = 2)
    private BigDecimal temperaturaAr;

    @Column(name = "DS_CONDICAO_CLIMA", length = 50)
    private String condicaoClima;

    @Column(name = "DT_COLETA", nullable = false)
    private LocalDateTime dataColeta;

    @PrePersist
    protected void onCreate() {
        this.dataColeta = LocalDateTime.now();
    }
}