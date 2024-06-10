import { useRequest } from '@/api/base/useRequest.js'

export async function GetSetupGeneralAsync() {
  const result = {
    is_success: true,
    general: {},
    error: null
  }

  await useRequest
    .get('general')
    .then((res) => {
      result.general = res.data.data.general
      result.general.timing.delay_typing = res.data.data.general.timing.delay_typing / 1000
      result.general.timing.idle_duration = res.data.data.general.timing.idle_duration / 1000 / 60
      result.general.helpdesk.mail_helpdesk_content_old = JSON.parse(
        res.data.data.general.helpdesk.mail_helpdesk_content
      )
    })
    .catch((err) => {
      result.is_success = false
      result.error = err.response?.data
    })

  return result
}

export async function PatchSetupGeneralAsync(generalModel) {
  const Payload = JSON.stringify(generalModel)

  const result = {
    is_success: true,
    error: null
  }

  await useRequest.patch('general', Payload).catch((err) => {
    result.is_success = false
    result.error = err.response.data
  })

  return result
}