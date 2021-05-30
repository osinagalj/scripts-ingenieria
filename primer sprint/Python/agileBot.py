


import json
from typing import Callable

from Server.mundo_sintetico.event_handling import event_handling

"""
Clase que recibe y publica eventos en el mundo sintetico. Esta clase estara corriendo todo el tiempo en el mundo sintetico.
Autor: Lautaro.
"""

class AgileBot:

    tareas = []
    reuniones = []
    velocity = 0.0
    skills = []
    # personalidad

    def __init__(self):
        self.publisher = event_handling.EventPublisher("log_eventos")
        self.init_consumer()

    def init_consumer(self):
        """
            Genera las suscripciones a los eventos de control del proceso
        """
        consumer = event_handling.EventSubscriber("log_eventos")
        consumer.subscribe("CambioEstadoUserStory", self._get_callback_userStory())
        consumer.subscribe("FasePorIniciar", self._get_callback_strategies())
        consumer.start_listening()

    def _get_callback(self, func: Callable) -> Callable:
        """Crea el callback a ejecutar cuando se captura un evento que genera
        deadlines.


        """

        def callback(ch, method, properties, body):
            """Encapsula la funcion recibida en una funcion con los parametros
            requeridos para un callback.

            """
            #escupir en un balde el evento para que lo levante el otro thred

            func(self, json.loads(body))

        return callback
    def _get_callback_userStory(self) -> Callable:
        """
            param: eventName: nombre del evento suscripto
                   eventStrategy: nombre de la Strategy utilizada
                   eventPublish: nombre del evento a publicar luego de procesar las strategies
        """
        def callback(ch, method, properties, body):
            print("CALLBACK")
            event = json.loads(body)
            print(event)
            #self.publisher.publish("hola", {"Data": "sacatukaka"})
        return callback

    def _get_callback_ceremonyStart(self) -> Callable:
        def callback(ch, method, properties, body):
            print("Ceremonia detectada")
            event = json.loads(body)
        return callback


# Main
AgileBot()
