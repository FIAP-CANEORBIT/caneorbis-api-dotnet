package br.com.fiap.tdspo.gsolution.caneorbit.api.dto.request;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Positive;
import jakarta.validation.constraints.Size;
import java.math.BigDecimal;

public record PropriedadeRequestDTO(
        @NotBlank(message = "O nome da propriedade é obrigatório")
        @Size(min = 3, max = 100, message = "O nome deve ter entre 3 e 100 caracteres")
        String nome,

        @Size(max = 150, message = "A localização deve ter no máximo 150 caracteres")
        String localizacao,

        @Positive(message = "A área deve ser um valor positivo")
        BigDecimal areaHectare
) {}