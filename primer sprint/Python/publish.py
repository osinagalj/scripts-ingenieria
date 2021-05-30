from Server.mundo_sintetico.event_handling import event_handling

publisher = event_handling.EventPublisher("log_eventos")

publisher.publish("example", {"Data": "hola22"})


