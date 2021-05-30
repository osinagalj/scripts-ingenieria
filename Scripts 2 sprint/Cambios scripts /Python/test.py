import json
import threading
import time

from Server.mundo_sintetico.event_handling.event_handling import \
    EventSubscriber, EventPublisher


def func(ch, method, properties, body):
    print(json.loads(body))
    clase.publish("example", {"msg": "HOLA3"})



class Test:
    def __init__(self):
        self.listener = EventSubscriber("log_eventos")
        self.publisher = EventPublisher("log_eventos")

    def start_listening(self):
        self.listener.start_listening()

    def subscribe(self, evento, callback):
        print("Me subscribi a " + evento)
        self.listener.subscribe(evento, callback)

    def publish(self, evento, payload):
        print("Publique " + evento)
        self.publisher.publish(evento, payload)


clase = Test()
clase.subscribe("CambioEstadoUserStory", func)


print("Starts listening")
thread = threading.Thread(target=clase.start_listening)
thread.start()

clase.publish("example", {"msg": "HOLA7"})
#clase.publish("Evento1", {"msg": "HOLA"})




