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
