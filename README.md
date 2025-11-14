[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/IFF0cJF3)
# Prac2 Portal

# Sistema de Portales Interactivo
**Desarrollo Académico en Entornos 3D**

## Descripción General
Este proyecto constituye un prototipo funcional inspirado en mecánicas de portales.
---

## Mecánicas Principales

### Esquema de Control
El sistema incorpora un modelo de interacción estándar utilizado en entornos en primera persona:

- **Movimiento del jugador:** `W`, `A`, `S`, `D`
- **Disparo de portales:**  
  - Clic izquierdo: Portal azul  
  - Clic derecho: Portal naranja
- **Ajuste del tamaño del portal azul:** Rueda del ratón (entre **50%** y **200%**)

---

## Manipulación de Objetos

- **Recoger y soltar:** tecla `E`  
- **Lanzar objetos:** tecla `F`

El sistema registra la velocidad y dirección del movimiento del jugador antes de soltar un objeto, habilitando:

1. **Liberación estática**, sin impulso adicional  
2. **Liberación con transferencia de impulso**, reflejando movimientos rápidos (flicks) o la velocidad al correr

Esto permite una interacción más expresiva y un control granular sobre elementos físicos del entorno.

---

## Shaders y Efectos Visuales

### Shader de Lava
Se ha implementado un shader específico para lava, que:
- Destruye objetos en contacto
- Simula una superficie activa con retroalimentación visual clara
- Añade riesgo controlado al diseño del nivel

### Shader del Arma
El arma del jugador incorpora un shader que:
- Reacciona a las fuentes de luz del entorno
- Mejora la percepción de materialidad
- Aumenta la coherencia visual del conjunto





