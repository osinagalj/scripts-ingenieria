from datetime import datetime
import random
from event_handling import event_handling


class NegotiationParticipant():
    """
        This Role is responsible for executing all the actions corresponding to a negotiation between 2 or
        more AgileBots in order to agree on a meeting based on the profits of each time available to
        each teamMember

        @Author: Lautaro
    """

    def __init__(self, name):
        print("----------------  AgileBot: ", name, "se unio a la negociacion  --------")

        self.name = name
        self.free_times = dict()
        self.meeting = None
        self.participants = []
        self.current_proposal = 0
        self.initiator = False

        self.publisher = event_handling.EventPublisher("log_eventos")
        self.custom_events = {"propose_meeting", "start_negotiation", "proposal_utility", "end_negotiation"}

        self.set_times()
        self.set_participants()

    def start_negotiation(self, payload):
        """ Método para dar comienzo a la negociacion. Se encarga de buscar a los participantes y enviar la propuesta inicial
        @Author: Lautaro Osinaga.

        :param payload: diccionario con datos del evento.
        """
        self.initiator = True
        print(" El agileBot", self.name, "inicia la negociacion")
        return self.generate_proposal()  # cuando se maneje con roles el return no va

    def set_participants(self):
        # 1. todo Obtener todos los participantes.
        self.participants = ["Lautaro", "Juan"]

    def set_times(self):
        # 2. todo Obtener los horarios disponibles del teamMember y asignarle una utilidad (0 <= U <= 1)
        self.free_times = dict({random.uniform(0, 1): "18 del 06 de 2021 a las 13:00",
                                random.uniform(0, 1): "18 del 06 de 2021 a las 14:00",
                                random.uniform(0, 1): "18 del 06 de 2021 a las 15:00",
                                random.uniform(0, 1): "18 del 06 de 2021 a las 16:00",
                                random.uniform(0, 1): "18 del 06 de 2021 a las 17:00"
                                })

    def get_utility(self, time):
        """
        Gets the utility of a schedule for the same agileBot.
        :param time: time at which the utility must be found
        :return:
        """
        times = sorted(self.free_times.items())
        for x in range(0, len(times)):
            if time == times[x][1]:
                print("El AgileBot", self.name, "tiene el horario propuesto libre")
                return times[x][0]

        return -1

    def generate_proposal(self):
        """
        This method is responsible for generating a proposal based on the current_proposal and sending an event to all participants about it
        :return: the proposal (utility, schedule)
        """
        times = sorted(self.free_times.items())

        if self.current_proposal < len(times):
            print("Hay al menos una propuesta para generar por parte de ", self.name)
            proposal = times[self.current_proposal]
            proposal_time = datetime.strptime(proposal[1], '%d del %m de %Y a las %H:%M')
            current_utility = proposal[0]
            self.current_proposal = self.current_proposal + 1
            print("Propuesta actual = ", proposal_time)

            for x in range(0, len(self.participants)):
                participant = self.participants[x]
                print("se envia la propuesta a ", participant)
                self.publisher.publish("propose_meeting",
                                       {
                                           "time": proposal_time.utcnow().strftime("%Y-%m-%d %H:%M:%S"),
                                           "utility": current_utility,
                                           "duration": "15 min",
                                           "meeting_id": 1,  # self.meeting.get_id()
                                           "to": participant

                                       })
            return proposal  # eliminar desp

        else:
            """
            If the facilitator ran out of proposals, an event is sent to all those who were waiting for 
            a proposal so that they do not wait any longer.
            """
            self.desagreement_proposal()

    def receive_utility(self, payload):
        print("EL AgileBot ", self.name, " recibio la utilidad", payload)
        print(payload[0])
        my_utility = self.get_utility(payload[1])
        if my_utility < payload[0]:
            print(" Le toca proponer al agileBot ", self.name)

    def send_utility(self, time):
        """
        This method is in charge of notifying the usefulness of the agileBot for a specific time.
        :param time:
        :return:
        """
        z = self.get_utility(time)
        for x in range(0, len(self.participants)):
            participant = self.participants[x]
            self.publisher.publish("propose_meeting",
                                   {
                                       "time": time,
                                       "utility": z,
                                       "to": participant
                                   })
        return [z, time]

    def propose_meeting(self, payload):
        """
        This function is in charge of receiving a proposal and evaluating it. If you agree,
        it is accepted, otherwise it rejects
        :param payload:
        :return:
        """
        print("El AgileBot ", self.name, "recibio la propuesta", payload)

        proposal_time = datetime.strptime(payload[1], '%d del %m de %Y a las %H:%M')
        current_utility = payload[0]

        my_utility = self.get_utility(payload[1])
        print("----------   Evaluando la propuesta   ----------")
        if my_utility != -1:

            # calcular utilidad
            my_utility = self.get_utility(payload[1])
            print("Para", self.name, "la propuesta tiene una utilidad de: ", my_utility)
            print("La utilidad de la propuesta recibida es de: ", current_utility)

            # TODO si la utilidad me sirve acepto sino genero una proposal nueva
            if my_utility >= current_utility:
                print("Acepto la propuesta")
                self.agreement_proposal(self.name)
            else:
                #print("Rechazo la propuesta")
                self.desagreement_proposal()
                # self.receive_utility(self.send_utility(payload[1]))
                self.publisher.publish("propose_meeting",
                                       {
                                           "time": payload[1],
                                           "utility": my_utility,
                                           "to": "Lautaro"
                                       })
                # TODO evaluar si hay que eliminar o no la propuesta rechazada
                # self.generate_proposal()

        else:
            print("NO contiene el horario de la negociacion")
            # TODO haca hay que notificarle al facilitador que no tiene el horario disponible
            self.generate_proposal()

    def reject_proposal(self):
        print("propuesta rechazada")
        self.publisher.publish("end_negotiation",
                               {
                                   "to": "Lautaro"
                               })

    def desagreement_proposal(self):
        print(" ---------  NO se ha llegado a un acuerdo   ----------")
        print("La negociacion termino.")
        self.publisher.publish("end_negotiation",
                               {
                                   "to": "Lautaro"
                               })

    def agreement_proposal(self, payload):
        # en el payload tiene que venir la persona que acepto
        # todo tiene ue venir un payload con el id de la reunion para asignarsela a cada uno
        if self.initiator:  # si soy el que empezo la reunion
            # todo guardar la persona y el horario que acepto
            print(" X persona acepto Y horario")

        print(" ---------  Se llego a un acuerdo  ----------")
        print(" Creando reunion ...")


""" 
-------------------------------------------------------------------- 
------------------------------  Main  -------------------------------
---------------------------------------------------------------------
"""

t1 = NegotiationParticipant("Lautaro")
t2 = NegotiationParticipant("Juan")

proposal = t1.start_negotiation(2)
t2.propose_meeting(proposal)
