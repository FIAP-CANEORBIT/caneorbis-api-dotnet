package br.com.fiap.tdspo.gsolution.caneorbit.domain.model;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Entity
@Table(name = "T_ORB_DISPOSITIVO_IOT")
public class DispositivoIot {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ID_DISPOSITIVO", nullable = false)
    private Long id;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "ID_FIELD")
    private Field field;

    @Column(name = "DS_MAC_ADDRESS", nullable = false, unique = true, length = 17)
    private String macAddress;

    @Column(name = "NM_APELIDO", length = 50)
    private String apelido;

    @Column(name = "VL_LATITUDE", precision = 10, scale = 8)
    private BigDecimal latitude;

    @Column(name = "VL_LONGITUDE", precision = 11, scale = 8)
    private BigDecimal longitude;

    @Column(name = "DS_STATUS_DISPOSITIVO", nullable = false, length = 20)
    private String statusDispositivo;

    @Column(name = "DT_INSTALACAO", nullable = false)
    private LocalDate dataInstalacao;

    @OneToMany(mappedBy = "dispositivo", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    private List<LeituraSensor> leituras = new ArrayList<>();

    @OneToMany(mappedBy = "dispositivo", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    private List<DadoSatelite> dadosSatelite = new ArrayList<>();
}