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
@Table(name = "T_ORB_LEITURA_SENSOR")
public class LeituraSensor {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ID_LEITURA", nullable = false)
    private Long id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "ID_DISPOSITIVO", nullable = false)
    private DispositivoIot dispositivo;

    @Column(name = "VL_UMIDADE_SOLO", nullable = false, precision = 10, scale = 2)
    private BigDecimal umidadeSolo;

    @Column(name = "VL_TEMPERATURA", nullable = false, precision = 10, scale = 2)
    private BigDecimal temperatura;

    @Column(name = "VL_PH_SOLO", precision = 4, scale = 2)
    private BigDecimal phSolo;

    @Column(name = "DT_LEITURA", nullable = false)
    private LocalDateTime dataLeitura;

    @PrePersist
    protected void onCreate() {
        this.dataLeitura = LocalDateTime.now();
    }
}