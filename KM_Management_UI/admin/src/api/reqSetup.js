import { useRequest } from '@/api/base/useRequest.js'


export async function GetAssistantProfileAsync() {
  const result = {
    is_success: true,
    profile: null,
    error: null
  }

  await useRequest
    .get(`AssistantProfile/GetAssistantProfile`)
    .then((res) => {
      result.profile = res.data.data.assistant_profile
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })

  return result
}

  export async function PostAssistantProfileAsync(Payload) {
    
    const result = {
      is_success: true,
      error: null
    }

    const config = {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    };
  
    await useRequest.post('assistantProfile/addAssistantProfile', Payload, config).catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })
  
    return result

  }


  export async function GetActiveMessageAsync(type) {
    const result = {
      is_success: true,
      active_message: null,
      error: null
    }

      const Payload = JSON.stringify({
      Type: type,
      })
  
    await useRequest
      .post(`Message/GetMessage`, Payload)
      .then((res) => {
        result.active_message = res.data.data.active_message
      })
      .catch((err) => {
        result.is_success = false
        result.error = err.response.data
      })
  
    return result
  }

  export async function PostMessageAsync(type, contents) {
    const Payload = JSON.stringify({
      Type: type,
      Contents: contents,
    })
  
    const result = {
      is_success: true,
      error: null
    }
    await useRequest.post('Message/AddNewMessage', Payload).catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })
  
    return result
  }

  export async function PatchMessageAsync(
    type,
    sequence,
    contents,
    is_active,
  ) {
    const Payload = JSON.stringify({
      Type: type,
      Sequence: sequence,
      Contents: contents,
      Is_Active: is_active,
    })
  
    const result = {
      is_success: true,
      error: null
    }
  
    await useRequest.patch('Message/UpdateSetupMessage', Payload).catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })
  
    return result
  }

  export async function PatchSequenceMessageAsync(
    type,
    current_sequence,
    new_sequence,
  ) {
    const Payload = JSON.stringify({
      Type: type,
      Current_Sequence: current_sequence,
      New_Sequence: new_sequence,
    })
  
    const result = {
      is_success: true,
      error: null
    }
  
    await useRequest.patch('Message/UpdateSequenceMessage', Payload).catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })
  
    return result
  }

  export async function DeleteMessageAsync(
    type,
    sequence,
  ) {
    const Payload = JSON.stringify({
      Type: type,
      Sequence: sequence,
    })
  
    const result = {
      is_success: true,
      error: null
    }
  
    await useRequest.patch('Message/SoftDeleteMessage', Payload).catch((err) => {
      result.is_success = false
      result.error = err.response.data
    })
  
    return result
  }

