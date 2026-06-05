package br.com.fiap.tdspo.gsolution.caneorbit.domain.service;

import br.com.fiap.tdspo.gsolution.caneorbit.domain.model.Usuario;

public interface TokenService {
    String generateToken(Usuario usuario);
    String getSubject(String tokenJWT);
}