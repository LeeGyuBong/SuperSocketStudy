﻿using MessagePack;
using SuperSocketServer.Manager;
using SuperSocketServer.Network.TCP;
using SuperSocketShared.Packet;
using System;

namespace SuperSocketServer.PacketHandler
{
    public class CommonHandler
    {
        public void ProcessLoginReq(SocketSession session, string packetData)
        {
            PKLoginReq packet = MessagePackSerializer.Deserialize<PKLoginReq>(Convert.FromBase64String(packetData));
            if (packet == null)
                return;

            session.SetUserName(packet.UserName);

            PKLoginAck ack = new PKLoginAck();
            ack.UID = session.UID;
            session.SendPacket(PacketID.LoginAck, ack);
        }

        public void ProcessLoadCompletedReq(SocketSession session, string packetData)
        {
             PKLoadCompletedReq packet = MessagePackSerializer.Deserialize<PKLoadCompletedReq>(Convert.FromBase64String(packetData));
            if (packet == null)
                return;

            PKBroadcastChatAck ack = new PKBroadcastChatAck();
            ack.Sender = "";
            ack.Message = $"{session.UserName}님이 접속했습니다.";

            SocketPacket ackPacket = new SocketPacket((int)PacketID.BroadcastChatAck);
            ackPacket.Data = Convert.ToBase64String(MessagePackSerializer.Serialize(ack));

            SessionManager.Instance.SendAll(ackPacket.GetBytes());
        }

        public void ProcessChatReq(SocketSession session, string packetData)
        {
            PKChatReq packet = MessagePackSerializer.Deserialize<PKChatReq>(Convert.FromBase64String(packetData));
            if (packet == null)
                return;

            PKBroadcastChatAck ack = new PKBroadcastChatAck();
            ack.Sender = packet.Sender;
            ack.Message = packet.Message;

            SocketPacket ackPacket = new SocketPacket((int)PacketID.BroadcastChatAck);
            ackPacket.Data = Convert.ToBase64String(MessagePackSerializer.Serialize(ack));

            SessionManager.Instance.SendAll(ackPacket.GetBytes());
        }
    }
}
