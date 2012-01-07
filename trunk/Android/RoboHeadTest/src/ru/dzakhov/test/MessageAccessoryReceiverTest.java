package ru.dzakhov.test;

import java.util.List;

import ru.dzakhov.MessageAccessoryReceiver;

import junit.framework.TestCase;

/**
 * ����� ������ MessageAccessoryReceiver.
 * @author �������
 *
 */
public final class MessageAccessoryReceiverTest extends TestCase {
	/**
	 * ���� 1 ������ onAccessoryMessage.
	 */
	public void testOnAccessoryMessage1() {
		MessageAccessoryReceiver messageAccessoryReceiver = new MessageAccessoryReceiver(null);
		String messages = "";
		List<String> messageList = messageAccessoryReceiver.getMessagesFromBuffer(messages.getBytes());
		assertEquals(0, messageList.size());
	}

	/**
	 * ���� 2 ������ onAccessoryMessage.
	 */
	public void testOnAccessoryMessage2() {
		MessageAccessoryReceiver messageAccessoryReceiver = new MessageAccessoryReceiver(null);
		String messages = "HT0";
		List<String> messageList = messageAccessoryReceiver.getMessagesFromBuffer(messages.getBytes());
		assertEquals(0, messageList.size());
	}

	/**
	 * ���� 3 ������ onAccessoryMessage.
	 */
	public void testOnAccessoryMessage3() {
		MessageAccessoryReceiver messageAccessoryReceiver = new MessageAccessoryReceiver(null);
		String messages = "HT000";
		List<String> messageList = messageAccessoryReceiver.getMessagesFromBuffer(messages.getBytes());
		assertEquals(1, messageList.size());
		assertEquals("HT000", messageList.get(0));
	}

	/**
	 * ���� 4 ������ onAccessoryMessage.
	 */
	public void testOnAccessoryMessage4() {
		MessageAccessoryReceiver messageAccessoryReceiver = new MessageAccessoryReceiver(null);
		String messages = "HT001FR000FL000";
		List<String> messageList = messageAccessoryReceiver.getMessagesFromBuffer(messages.getBytes());
		final int numOfMessages = 3;
		assertEquals(numOfMessages, messageList.size());
		assertEquals("HT001", messageList.get(0));
		assertEquals("FR000", messageList.get(1));
		assertEquals("FL000", messageList.get(2));
	}

	/**
	 * ���� 5 ������ onAccessoryMessage.
	 */
	public void testOnAccessoryMessage5() {
		MessageAccessoryReceiver messageAccessoryReceiver = new MessageAccessoryReceiver(null);
		
		String messages = "HT001FR0";
		List<String> messageList = messageAccessoryReceiver.getMessagesFromBuffer(messages.getBytes());
		assertEquals(1, messageList.size());
		assertEquals("HT001", messageList.get(0));
		
		messages = "00FL000";
		messageList = messageAccessoryReceiver.getMessagesFromBuffer(messages.getBytes());
		assertEquals(2, messageList.size());
		assertEquals("FR000", messageList.get(0));
		assertEquals("FL000", messageList.get(1));
	}
}
